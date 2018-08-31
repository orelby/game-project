import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { GameLibraryComponent } from './game-library.component';

describe('GameLibraryComponent', () => {
  let component: GameLibraryComponent;
  let fixture: ComponentFixture<GameLibraryComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ GameLibraryComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(GameLibraryComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
