import { Component, EventEmitter, inject, OnInit, Output, signal } from '@angular/core';
import { RolePermissionsService } from '../../services/role-permissions.service';
import { Role } from '../../types/role.type';
import { RolePermissions } from '../../types/role_permissions.type';
import { catchError, forkJoin, switchMap, tap } from 'rxjs';
import { Permission } from '../../types/permission.type';
import { ReactiveFormsModule, AbstractControl, FormControl, FormGroup, ValidationErrors, Validators, FormArray } from '@angular/forms';
import { UsersService } from '../../services/users.service';
import { CreateUser } from '../../types/create_user.type';
import { RolePermissionsUpdate } from '../../types/role_permissions_update.type';

@Component({
  selector: 'app-add-user-popup-form',
  imports: [ReactiveFormsModule],
  templateUrl: './add-user-popup-form.component.html',
  styleUrl: './add-user-popup-form.component.css'
})
export class AddUserPopupFormComponent implements OnInit {
  rolePermissionsService = inject(RolePermissionsService);
  userService = inject(UsersService);

  @Output() close = new EventEmitter<void>();
  @Output() userAdded = new EventEmitter<void>();

  roles = signal<Array<Role>>([]);
  permissions = signal<Array<Permission>>([]);
  rolePermissionsList = signal<Array<RolePermissions>>([]);

  form!: FormGroup;

  ngOnInit(): void {
    this.initForm();
    this.loadAllData();
  }

  onCloseClick() {
    this.close.emit();
  }

  private loadAllData(): void {
    forkJoin({
      roles: this.rolePermissionsService.getRolesFromAPI().pipe(
        catchError(error => {
          console.error('Error loading roles:', error);
          throw new Error('Failed to load roles');
        })
      ),
      permissions: this.rolePermissionsService.getPermissionsFromAPI().pipe(
        catchError(error => {
          console.error('Error loading permissions:', error);
          throw new Error('Failed to load permissions');
        })
      )
    }).pipe(
      tap(result => {
        this.roles.set(result.roles);
        this.permissions.set(result.permissions);
      }),

      switchMap(result => {
        const rolePermissionsRequests = result.roles.map(role => 
          this.rolePermissionsService.getRolePermissionsFromAPI(role.id).pipe(
            catchError(error => {
              console.error(`Error loading permissions for role ${role.id}:`, error);
              throw new Error(`Failed to load permissions for role: ${role.roleName}`);
            })
          )
        );
        
        return forkJoin(rolePermissionsRequests);
      }),

      tap(rolePermissionsList => {
        this.rolePermissionsList.set(rolePermissionsList);

        this.buildPermissionsArray(rolePermissionsList);
      })
    ).subscribe({
      next: () => {},
      error: (error: Error) => {
        console.error('Data loading failed:', error);
      }
    });
  }

  private initForm() {
    this.form = new FormGroup({
      user_id:          new FormControl('', [Validators.required, this.guidValidator]),
      first_name:       new FormControl('', Validators.required),
      last_name:        new FormControl('', Validators.required),
      email:            new FormControl('', [Validators.required, Validators.email]),
      phone:            new FormControl('', this.mobileValidator),
      role_id:          new FormControl('select', [ this.defaultSelectValidator ]),
      username:         new FormControl('', Validators.required),
      password:         new FormControl('', Validators.required),
      confirm_password: new FormControl('', Validators.required),
    
      permissions:      new FormArray([])
    }, {
      validators: [ this.matchPasswordsValidator ]
    });
  }

  private buildPermissionsArray(rows: RolePermissions[]) {
    const arr = rows.map(rp => {
      const permsFA = new FormArray(
        rp.permissions.map(p =>
          new FormGroup({
            permissionId: new FormControl(p.id),
            permissionName: new FormControl(p.permissionName),
            isSelected:     new FormControl(p.isSelected)
          })
        )
      );
  
      return new FormGroup({
        roleName: new FormControl(rp.roleName),
        roleId: new FormControl(rp.roleId),
        perms:    permsFA
      });
    });
  
    this.form.setControl('permissions', new FormArray(arr));
  }

  get permissionsArray() {
    return this.form.get('permissions') as FormArray;
  }

  updatePermission(roleIndex: number, permIndex: number) {
    const rolesFA = this.form.get('permissions') as FormArray;
    const roleFG = rolesFA.at(roleIndex) as FormGroup;
    const permsFA = roleFG.get('perms') as FormArray;
    const permFG = permsFA.at(permIndex) as FormGroup;
  
    const current = permFG.get('isSelected')!.value;
    permFG.get('isSelected')!.setValue(!current);
  
    const list = this.rolePermissionsList();
    list[roleIndex].permissions[permIndex].isSelected = !current;
    this.rolePermissionsList.set(list);
  }
  
  private guidValidator(ctrl: AbstractControl): ValidationErrors | null {
    const val: string = ctrl.value;
    if (!val) {
      return null;
    }

    const regex = /^[0-9A-Fa-f]{8}\b-[0-9A-Fa-f]{4}\b-[1-5][0-9A-Fa-f]{3}\b-[89ABab][0-9A-Fa-f]{3}\b-[0-9A-Fa-f]{12}$/;
    return regex.test(val) ? null : { invalidGuid: true };
  }

  private defaultSelectValidator(ctrl: AbstractControl): ValidationErrors | null {
    return ctrl.value !== 'select' ? null : { defaultRole: true };
  }

  private matchPasswordsValidator(group: AbstractControl): ValidationErrors | null {
    const pw  = group.get('password')?.value;
    const cpw = group.get('confirm_password')?.value;
    return pw && cpw && pw !== cpw
      ? { passwordMismatch: true }
      : null;
  }

  private mobileValidator(ctrl: AbstractControl): ValidationErrors | null {
    const val: string = ctrl.value;
    if (!val) {
      return null;
    }

    const re = /^\+?\d{7,15}$/;
    return re.test(val)
      ? null
      : { invalidMobile: true };
  }
  

  onSubmit() {
    if (this.form.invalid) {
      console.log(this.form.value);
      this.form.markAllAsTouched();
      return;
    }

    const raw = this.form.value;

    const userPayload: CreateUser = {
      id: raw.user_id,
      firstName: raw.first_name,
      lastName: raw.last_name,
      username: raw.username,
      email: raw.email,
      password: raw.password,
      phone: raw.phone || null,
      roleId: raw.role_id
    }
    
    const updates: RolePermissionsUpdate[] = raw.permissions.map((row: any) => ({
      roleId: row.roleId,
      permissionIds: (row.perms as any[])
        .filter(p => p.isSelected)
        .map(p => p.permissionId)
    }));

    updates.forEach(u => {
      this.rolePermissionsService
        .updateRolePermissionsFromAPI(u)
        .subscribe({
          next: () => { },
          error: err => console.error('perm update failed', err)
        });
    });

    this.userService.createUserFromAPI(userPayload).subscribe({
      next: () => {
        this.userAdded.emit();
        this.close.emit();
      },
      error: err => console.error(err)
    });
  }
}
