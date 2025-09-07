import { TestBed } from '@angular/core/testing';

import { GamesFromFollowersTwitchService } from './games-from-followers-twitch.service';

describe('GamesFromFollowersTwitchService', () => {
  let service: GamesFromFollowersTwitchService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(GamesFromFollowersTwitchService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
