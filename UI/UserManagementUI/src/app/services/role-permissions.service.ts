import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { Role } from '../types/role.type';
import { Permission } from '../types/permission.type';
import { RolePermissions } from '../types/role_permissions.type';
import { RolePermissionsUpdate } from '../types/role_permissions_update.type';

@Injectable({
  providedIn: 'root'
})
export class RolePermissionsService {
  http = inject(HttpClient);

  getRolesFromAPI() {
    const url = "https://localhost:7110/api/roles";
    return this.http.get<Array<Role>>(url);
  }

  getPermissionsFromAPI() {
    const url = "https://localhost:7110/api/permissions";
    return this.http.get<Array<Permission>>(url);
  }

  getRolePermissionsFromAPI(roleId: string) {
    const url = `https://localhost:7110/api/rolepermissions/${roleId}`;
    return this.http.get<RolePermissions>(url);
  }

  updateRolePermissionsFromAPI(rolePermissionsRequest: RolePermissionsUpdate) {
    const url = "https://localhost:7110/api/rolepermissions";
    return this.http.put(url, rolePermissionsRequest);
  }
}
