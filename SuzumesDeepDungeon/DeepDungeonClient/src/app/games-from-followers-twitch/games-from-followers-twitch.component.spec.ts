import { ComponentFixture, TestBed } from '@angular/core/testing';

import { GamesFromFollowersTwitchComponent } from './games-from-followers-twitch.component';

describe('GamesFromFollowersTwitchComponent', () => {
  let component: GamesFromFollowersTwitchComponent;
  let fixture: ComponentFixture<GamesFromFollowersTwitchComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [GamesFromFollowersTwitchComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(GamesFromFollowersTwitchComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
