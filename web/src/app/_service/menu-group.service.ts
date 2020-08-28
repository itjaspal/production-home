import { environment } from '../../environments/environment';
import { CommonSearchView } from '../_model/common-search-view';
import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { MenuGroupSearchView, MenuGroupView } from '../_model/menugroup';

@Injectable({
  providedIn: 'root'
})
export class MenuGroupService {

  constructor(private http: HttpClient) { }

  public async search(_model: MenuGroupSearchView) {
    return await this.http.post<CommonSearchView<MenuGroupView>>(environment.API_URL + 'master-menu-group/postSearch', _model).toPromise();
  }

  public async create(_model: MenuGroupView) {
    return await this.http.post<number>(environment.API_URL + 'master-menu-group/postCreate', _model).toPromise();
  }

  public async update(_model: MenuGroupView) {
    return await this.http.post<number>(environment.API_URL + 'master-menu-group/postUpdate', _model).toPromise();
  }

  public async getInfo(_id: string) {
    return await this.http.get<MenuGroupView>(environment.API_URL + 'master-menu-group/getInfo/' + _id).toPromise();
  }

}