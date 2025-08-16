import { ComponentFixture, TestBed } from '@angular/core/testing';

import { GamesRanking } from './games-ranking';

describe('GamesRanking', () => {
  let component: GamesRanking;
  let fixture: ComponentFixture<GamesRanking>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [GamesRanking]
    })
    .compileComponents();

    fixture = TestBed.createComponent(GamesRanking);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
