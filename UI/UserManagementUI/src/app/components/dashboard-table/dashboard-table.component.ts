import { Component, inject, Input, OnInit, signal } from '@angular/core';
import { UsersService } from '../../services/users.service';
import { UsersData } from '../../types/user_data.type';

@Component({
  selector: 'app-dashboard-table',
  imports: [],
  templateUrl: './dashboard-table.component.html',
  styleUrl: './dashboard-table.component.css'
})
export class DashboardTableComponent {
  userService = inject(UsersService);
  @Input() usersData!: UsersData;
}
