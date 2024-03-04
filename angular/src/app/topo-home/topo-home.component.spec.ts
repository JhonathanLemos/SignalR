import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TopoHomeComponent } from './topo-home.component';

describe('TopoHomeComponent', () => {
  let component: TopoHomeComponent;
  let fixture: ComponentFixture<TopoHomeComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [TopoHomeComponent]
    });
    fixture = TestBed.createComponent(TopoHomeComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
