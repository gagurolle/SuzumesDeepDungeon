import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ApiFormComponent } from './api-form.component';

describe('ApiFormComponent', () => {
  let component: ApiFormComponent;
  let fixture: ComponentFixture<ApiFormComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ApiFormComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ApiFormComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
