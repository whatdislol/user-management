import { Routes } from '@angular/router';

export const routes: Routes = [
    {
        path: "",
        pathMatch: "full",
        redirectTo: "/dashboard"
    },
    {
        path: "dashboard",
        loadComponent: () => {
            return import("./dashboard/dashboard.component").then((m) => m.DashboardComponent)
        }
    },
    {
        path: "users",
        loadComponent: () => {
            return import("./users/users.component").then((m) => m.UsersComponent)
        }
    }
];
