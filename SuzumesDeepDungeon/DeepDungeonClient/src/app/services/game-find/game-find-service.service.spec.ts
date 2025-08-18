import { TestBed } from '@angular/core/testing';
import { GameFindServiceService } from './game-find-service.service';



describe('GameFindServiceService', () => {
  let service: GameFindServiceService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(GameFindServiceService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
