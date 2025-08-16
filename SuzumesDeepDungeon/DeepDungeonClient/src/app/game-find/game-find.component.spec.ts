import { ComponentFixture, TestBed } from '@angular/core/testing';

import { GameFindComponent } from './game-find.component';

describe('GameFindComponent', () => {
  let component: GameFindComponent;
  let fixture: ComponentFixture<GameFindComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [GameFindComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(GameFindComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
