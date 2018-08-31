import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';

import { AppRoutingModule } from './app-routing.module';

import { NgbButtonsModule } from '@ng-bootstrap/ng-bootstrap';

import { AppComponent } from './components/app.component';
import { HomeComponent } from './components/home/home.component';
import { GameLibraryComponent } from './components/game-library/game-library.component';
import { TicTacToeComponent } from './components/tic-tac-toe/tic-tac-toe.component';

import { TicTacToeAiService } from './services/games/tic-tac-toe/ai.service';
import { TicTacToePvpService } from './services/games/tic-tac-toe/pvp.service';

@NgModule({
  imports: [
    BrowserModule,
    AppRoutingModule,
    FormsModule,
    NgbButtonsModule
  ],
  declarations: [
    AppComponent,
    GameLibraryComponent,
    TicTacToeComponent,
    HomeComponent
  ],
  providers: [
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
