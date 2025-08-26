import { Routes } from '@angular/router';
import { LoginComponent } from './login/login';
import { GameListComponent } from './game-list/game-list';
import { ProfileComponent } from './app-profile/app-profile';
import { AuthGuard } from './services/auth/auth-guard/auth-guard';
import { MinecraftServerComponent } from './minecraft-server/minecraft-server.component';



export const routes: Routes = [
  {
    path: 'login',
    component: LoginComponent
  },
  {
    path: '',
    component: GameListComponent // Главная страница доступна всем
  },
  { path: 'profile', component: ProfileComponent, canActivate: [AuthGuard] },
  { path: 'minecraft', component: MinecraftServerComponent}, // Профиль доступен только авторизованным пользователям
];
