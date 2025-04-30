import { inject, Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { UsersData } from '../types/user_data.type';
import { User } from '../types/user.type';
import { CreateUser } from '../types/create_user.type';


export interface UserQueryOptions {
  pageNumber?: number;
  pageSize?: number;
  orderBy?: string;
  orderDirection?: string;
  searchQuery?: string;
}

@Injectable({
  providedIn: 'root'
})
export class UsersService {
  private readonly http = inject(HttpClient);
  private readonly apiUrl = 'https://localhost:7110/api/users';
  
  getUsersDataFromAPI(opts: UserQueryOptions={}) {
    let params = new HttpParams();

    if (opts.pageNumber != null) {
      params = params.set('pageNumber', opts.pageNumber.toString());
    }
    if (opts.pageSize != null) {
      params = params.set('pageSize', opts.pageSize.toString());
    }
    if (opts.orderBy) {
      params = params.set('orderBy', opts.orderBy);
    }
    if (opts.orderDirection) {
      params = params.set('orderDirection', opts.orderDirection);
    }
    if (opts.searchQuery) {
      params = params.set('searchQuery', opts.searchQuery);
    }

    return this.http.get<UsersData>(this.apiUrl, { params });
  }

  createUserFromAPI(user: CreateUser) {
    return this.http.post<User>(this.apiUrl, user);
  }
}
