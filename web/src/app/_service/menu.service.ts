import { environment } from '../../environments/environment';
import { CommonSearchView } from '../_model/common-search-view';
import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { MenuSearchView, MenuView } from '../_model/menu';


@Injectable({
  providedIn: 'root' 
})
export class MenuService {

  constructor(private http: HttpClient) { }

  public async search(_model: MenuSearchView) {
    return await this.http.post<CommonSearchView<MenuView>>(environment.API_URL + 'master-menu/postSearch', _model).toPromise();
  }

  public async create(_model: MenuView) {
    return await this.http.post<number>(environment.API_URL + 'master-menu/postCreate', _model).toPromise();
  }

  public async update(_model: MenuView) {
    return await this.http.post<number>(environment.API_URL + 'master-menu/postUpdate', _model).toPromise();
  }

  public async getInfo(_id: string) {
    return await this.http.get<MenuView>(environment.API_URL + 'master-menu/getInfo/' + _id).toPromise();
  }

}