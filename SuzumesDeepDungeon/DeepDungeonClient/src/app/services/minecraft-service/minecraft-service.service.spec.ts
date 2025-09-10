import { TestBed } from '@angular/core/testing';
import { MinecraftService } from './minecraft-service.service';

describe('MinecraftServiceService', () => {
  let service: MinecraftService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(MinecraftService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
