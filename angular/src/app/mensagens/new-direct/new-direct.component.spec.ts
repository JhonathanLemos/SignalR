import { ComponentFixture, TestBed } from '@angular/core/testing';

import { NewDirectComponent } from './new-direct.component';

describe('NewDirectComponent', () => {
  let component: NewDirectComponent;
  let fixture: ComponentFixture<NewDirectComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [NewDirectComponent]
    });
    fixture = TestBed.createComponent(NewDirectComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
