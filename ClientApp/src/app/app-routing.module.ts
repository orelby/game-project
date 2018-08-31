import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { AppComponent } from './components/app.component';
import { GameLibraryComponent } from './components/game-library/game-library.component';
import { TicTacToeComponent } from './components/tic-tac-toe/tic-tac-toe.component';
import { HomeComponent } from './components/home/home.component';

const routes: Routes = [
    { path: '', component: HomeComponent, pathMatch: 'full' },
    { path: 'games', component: GameLibraryComponent },
    { path: 'games/tic-tac-toe', component: TicTacToeComponent }
];

@NgModule({
    imports: [RouterModule.forRoot(routes)],
    exports: [RouterModule]
})
export class AppRoutingModule { }
