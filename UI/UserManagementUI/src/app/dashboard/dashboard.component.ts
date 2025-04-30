import { Component, inject, OnInit, signal } from '@angular/core';
import { DashboardToolbarComponent } from "../components/dashboard-toolbar/dashboard-toolbar.component";
import { DashboardNavComponent } from "../components/dashboard-nav/dashboard-nav.component";
import { UsersService } from '../services/users.service';
import { DashboardTableComponent } from "../components/dashboard-table/dashboard-table.component";
import { AddUserPopupFormComponent } from "../components/add-user-popup-form/add-user-popup-form.component";
import { UsersData } from '../types/user_data.type';
import { catchError } from 'rxjs';

@Component({
  selector: 'app-dashboard',
  imports: [DashboardToolbarComponent, DashboardNavComponent, DashboardTableComponent, AddUserPopupFormComponent],
  templateUrl: './dashboard.component.html',
  styleUrl: './dashboard.component.css'
})
export class DashboardComponent implements OnInit {
  userService = inject(UsersService);
  
  pageSizes = signal<Array<number>>([2, 3, 5, 10, 15, 20]);
  selectedPageSize = signal<number>(10);
  currentPageNumber = signal<number>(1);
  
  searchQuery = signal<string>('');
  
  showPopup = signal<boolean>(false);
  usersData = signal<UsersData>({
      users: [],
      pageNumber: 0,
      pageSize: 0,
      totalCount: 0
    });
  
  get lowerBound(): number {
    if (this.usersData().totalCount === 0) return 0;
    return (this.currentPageNumber() - 1) * this.selectedPageSize() + 1;
  }
  
  get upperBound(): number {
    const calculatedUpper = this.currentPageNumber() * this.selectedPageSize();
    return Math.min(calculatedUpper, this.usersData().totalCount);
  }
  
  ngOnInit(): void {
    this.loadUsers();
  }

  openPopup(): void {
    this.showPopup.set(true);
  }

  closePopup(): void {
    this.showPopup.set(false);
  }

  onUserAdded(): void {
    this.loadUsers();
  }

  loadUsers(): void {
    this.userService
      .getUsersDataFromAPI({
        pageNumber: this.currentPageNumber(),
        pageSize: this.selectedPageSize(),
        searchQuery: this.searchQuery()
      })
      .pipe(
        catchError((err) => {
          console.log(err);
          throw err;
        })
      )
      .subscribe((data) => {
        this.usersData.set(data);
        this.currentPageNumber.set(data.pageNumber);
      });
  }

  handlePageSizeChange(newSize: number): void {
    this.selectedPageSize.set(newSize);
    this.currentPageNumber.set(1); 
    this.loadUsers();
  }
  
  handlePrevPage(): void {
    if (this.currentPageNumber() > 1) {
      this.currentPageNumber.update(page => page - 1);
      this.loadUsers();
    }
  }
  
  handleNextPage(): void {
    if (this.upperBound < this.usersData().totalCount) {
      this.currentPageNumber.update(page => page + 1);
      this.loadUsers();
    }
  }
  
  handleSearch(query: string): void {
    this.searchQuery.set(query);
    this.currentPageNumber.set(1); 
    this.loadUsers();
  }
}