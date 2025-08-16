import { Routes } from '@angular/router';
import { LoginComponent } from './login/login';
import { AuthGuard } from './auth-guard';
import { GameListComponent } from './game-list/game-list';
import { ProfileComponent } from './app-profile/app-profile';


export const routes: Routes = [
  {
    path: 'login',
    component: LoginComponent
  },
  {
    path: '',
    component: GameListComponent // Главная страница доступна всем
  },
  { path: 'profile', component: ProfileComponent, canActivate: [AuthGuard] }, // Профиль доступен только авторизованным пользователям
];
