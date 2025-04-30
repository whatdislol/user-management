import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AddUserPopupFormComponent } from './add-user-popup-form.component';

describe('AddUserPopupFormComponent', () => {
  let component: AddUserPopupFormComponent;
  let fixture: ComponentFixture<AddUserPopupFormComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [AddUserPopupFormComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(AddUserPopupFormComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
