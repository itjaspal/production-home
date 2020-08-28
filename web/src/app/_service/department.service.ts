import { environment } from './../../environments/environment';
import { CommonSearchView } from './../_model/common-search-view';
import { DepartmentSearchView, DepartmentView } from './../_model/department';
import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class DepartmentService {

  constructor(private http: HttpClient) { }

  public async search(_model: DepartmentSearchView) {
    return await this.http.post<CommonSearchView<DepartmentView>>(environment.API_URL + 'master-department/postSearch', _model).toPromise();
  }

  public async create(_model: DepartmentView) {
    return await this.http.post<number>(environment.API_URL + 'master-department/postCreate', _model).toPromise();
  }

  public async update(_model: DepartmentView) {
    return await this.http.post<number>(environment.API_URL + 'master-department/postUpdate', _model).toPromise();
  }

  public async getInfo(_departmentId: number) {
    return await this.http.get<DepartmentView>(environment.API_URL + 'master-department/getInfo/' + _departmentId).toPromise();
  }

  public async inquiryDepartment() {
    return await this.http.post(environment.API_URL + 'master-department/postInquiryDepartment', {}).toPromise();
  }
}