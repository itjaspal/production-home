import { Dropdownlist } from './../_model/dropdownlist';
import { environment } from './../../environments/environment';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import * as CryptoJS from 'crypto-js';

@Injectable({
  providedIn: 'root'
})
export class UserService {

  constructor(private http: HttpClient) { }

  public async get(username) {
    return await this.http.get(environment.API_URL + 'master-user/get/'+username).toPromise();
  }

  public async resetPassword(username) {
    let params = {
      username: username
    }
    return await this.http.post(environment.API_URL + 'master-user/reset-password',params).toPromise();
  }

  public async changePassword(username, oldPassword, password) {
    let params = {
      username: username,
      oldPassword: CryptoJS.SHA3(oldPassword).toString(),
      password: CryptoJS.SHA3(password).toString()
    } 
    return await this.http.post(environment.API_URL + 'master-user/change-password',params).toPromise();
  }

  public async searchUsers(params) {
    return await this.http.post<Dropdownlist[]>(environment.API_URL + 'master-user/inquiry',params).toPromise();
  }

  public async create(params) {
    return await this.http.post(environment.API_URL + 'master-user/create',params).toPromise();
  }

  public async update(params) {
    return await this.http.post(environment.API_URL + 'master-user/update',params).toPromise();
  }

  public async delete(params) {
    return await this.http.post(environment.API_URL + 'master-user/post/Delete',params).toPromise();
  }

  public async inquiryAllUser() {
    return await this.http.post(environment.API_URL + 'master-user/postInquiryAllUser',{}).toPromise();
  }
}
