import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DonateLinksComponent } from './donate-links.component';

describe('DonateLinksComponent', () => {
  let component: DonateLinksComponent;
  let fixture: ComponentFixture<DonateLinksComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [DonateLinksComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(DonateLinksComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
