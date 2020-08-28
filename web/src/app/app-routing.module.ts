import { RouterModule, Routes } from '@angular/router';
import { HomeComponent } from './home/home.component';
import { UserLoginComponent } from './user-login/user-login.component';



const AppRoutes: Routes = [
  { path: '', redirectTo: '/app/home', pathMatch: 'full' },
  { path: 'login', component: UserLoginComponent },
  // path: 'select-branch', component: PCSelectBranchComponent },
  

  { path: '**', redirectTo: '/app/home', pathMatch: 'full' },
];

export const AppRoutingModule = RouterModule.forRoot(AppRoutes, { useHash: true });

