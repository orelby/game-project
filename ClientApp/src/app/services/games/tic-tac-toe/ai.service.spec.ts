import { TestBed, inject } from '@angular/core/testing';

import { TicTacToeAiService } from './ai.service';

describe('TicTacToeAiService', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [TicTacToeAiService]
    });
  });

  it('should be created', inject([TicTacToeAiService], (service: TicTacToeAiService) => {
    expect(service).toBeTruthy();
  }));
});
