import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AppProfile } from './app-profile';

describe('AppProfile', () => {
  let component: AppProfile;
  let fixture: ComponentFixture<AppProfile>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [AppProfile]
    })
    .compileComponents();

    fixture = TestBed.createComponent(AppProfile);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
