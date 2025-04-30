import { Component, EventEmitter, Input, Output } from '@angular/core';
import { NgFor } from '@angular/common';

@Component({
  selector: 'app-dashboard-nav',
  standalone: true,
  imports: [],
  templateUrl: './dashboard-nav.component.html',
  styleUrl: './dashboard-nav.component.css'
})
export class DashboardNavComponent {
  @Input() pageSizes!: Array<number>;
  @Input() selectedPageSize!: number;
  @Input() currentPageNumber!: number;
  @Input() totalCount!: number;
  @Input() lowerBound!: number;
  @Input() upperBound!: number;
  
  @Output() pageSizeChange = new EventEmitter<number>();
  @Output() prevPage = new EventEmitter<void>();
  @Output() nextPage = new EventEmitter<void>();

  onPageSizeChange(event: Event): void {
    const newSize = Number((event.target as HTMLSelectElement).value);
    this.pageSizeChange.emit(newSize);
  }

  onPrevPage(): void {
    this.prevPage.emit();
  }

  onNextPage(): void {
    this.nextPage.emit();
  }

  canGoToPrevPage(): boolean {
    return this.currentPageNumber > 1;
  }

  canGoToNextPage(): boolean {
    return this.upperBound < this.totalCount;
  }
}