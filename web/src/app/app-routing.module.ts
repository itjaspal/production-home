import { RouterModule, Routes } from '@angular/router';
import { HomeComponent } from './home/home.component';
import { UserLoginComponent } from './user-login/user-login.component';
import { UserSelectBranchComponent } from './user-select-branch/user-select-branch/user-select-branch.component';



const AppRoutes: Routes = [
  { path: '', redirectTo: '/app/home', pathMatch: 'full' },
  { path: 'login', component: UserLoginComponent },
  { path: 'select-branch', component: UserSelectBranchComponent },
  

  { path: '**', redirectTo: '/app/home', pathMatch: 'full' },
];

export const AppRoutingModule = RouterModule.forRoot(AppRoutes, { useHash: true });

