import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SendSolicitationComponent } from './send-solicitation.component';

describe('SendSolicitationComponent', () => {
  let component: SendSolicitationComponent;
  let fixture: ComponentFixture<SendSolicitationComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [SendSolicitationComponent]
    });
    fixture = TestBed.createComponent(SendSolicitationComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
