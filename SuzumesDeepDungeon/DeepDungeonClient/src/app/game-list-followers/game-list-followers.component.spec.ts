import { ComponentFixture, TestBed } from '@angular/core/testing';

import { GameListFollowersComponent } from './game-list-followers.component';

describe('GameListFollowersComponent', () => {
  let component: GameListFollowersComponent;
  let fixture: ComponentFixture<GameListFollowersComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [GameListFollowersComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(GameListFollowersComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
