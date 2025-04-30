import { Component, EventEmitter, Output } from '@angular/core';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-dashboard-toolbar',
  standalone: true,
  imports: [FormsModule],
  templateUrl: './dashboard-toolbar.component.html',
  styleUrl: './dashboard-toolbar.component.css'
})
export class DashboardToolbarComponent {
  @Output() addUser = new EventEmitter<void>();
  @Output() search = new EventEmitter<string>();
    
  onAddUser(): void {
    this.addUser.emit();
  }
  
  onSearch(event: Event): void {
    const input = event.target as HTMLInputElement;
    this.search.emit(input.value);
  }
}