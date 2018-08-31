import { TestBed, inject } from '@angular/core/testing';

import { TicTacToePvpService } from './pvp.service';

describe('TicTacToePvpGameService', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [TicTacToePvpService]
    });
  });

  it('should be created', inject([TicTacToePvpService], (service: TicTacToePvpService) => {
    expect(service).toBeTruthy();
  }));
});
