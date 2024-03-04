import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CreateServerComponent } from './create-server.component';

describe('CreateServerComponent', () => {
  let component: CreateServerComponent;
  let fixture: ComponentFixture<CreateServerComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [CreateServerComponent]
    });
    fixture = TestBed.createComponent(CreateServerComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
