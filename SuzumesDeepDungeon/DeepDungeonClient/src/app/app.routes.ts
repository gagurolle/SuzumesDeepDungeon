import { Routes } from '@angular/router';
import { LoginComponent } from './login/login';
import { GameListComponent } from './game-list/game-list';
import { ProfileComponent } from './app-profile/app-profile';
import { AuthGuard } from './services/auth/auth-guard/auth-guard';
import { MinecraftServerComponent } from './minecraft-server/minecraft-server.component';
import { DonateLinksComponent } from './donate-links/donate-links.component';
import { GameListFollowersComponent } from './game-list-followers/game-list-followers.component';



export const routes: Routes = [
  {
    path: 'login',
    component: LoginComponent
  },
  {
    path: '',
    component: GameListComponent
  },
  { path: 'profile', component: ProfileComponent, canActivate: [AuthGuard] },
  { path: 'minecraft', component: MinecraftServerComponent},
  { path: 'donate-link', component: DonateLinksComponent},
  { path: 'game-list-followers', component: GameListFollowersComponent},
];
