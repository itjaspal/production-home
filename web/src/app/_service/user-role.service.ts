import { Dropdownlist } from './../_model/dropdownlist';
import { environment } from './../../environments/environment';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class UserRoleService {

  constructor(private http: HttpClient) { }

  public async getFunctionGroup(isPC) {
    //return await this.http.get(environment.API_URL + 'master-user-role/function-group/get/'+isPC).toPromise();
    return await this.http.get(environment.API_URL + 'master-user-role/get-function-group/'+isPC).toPromise();
    
  }

  public async get(userRoleId) {
    return await this.http.get(environment.API_URL + 'master-user-role/get/'+userRoleId).toPromise();
  }

  public async search(params) {
    return await this.http.post(environment.API_URL + 'master-user-role/inquiry',params).toPromise();
  }

  public async create(params) {
    return await this.http.post(environment.API_URL + 'master-user-role/create',params).toPromise();
  }

  public async update(params) {
    return await this.http.post(environment.API_URL + 'master-user-role/update',params).toPromise();
  }

  public async delete(params) {
    return await this.http.post(environment.API_URL + 'master-user-role/post/delete',params).toPromise();
  }
}
