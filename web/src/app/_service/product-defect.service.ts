import { DataQcCuttingView, ItemNoListView } from './../_model/product-defect';
import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment';
import { ProductDefectSearchView, ProductDefectView } from '../_model/product-defect';
import { CommonSearchView } from '../_model/common-search-view';

@Injectable({
  providedIn: 'root'
})
export class ProductDefectService {

  constructor(
    private http: HttpClient
  ) { }

  public async searchDataProductDefect(_model: ProductDefectSearchView) {
    return await this.http.post<ProductDefectView>(environment.API_URL + 'product-defect/postSearchDataProductDefect', _model).toPromise();
  }

  public async getItemNo(_entity: string , _porNo: string) {
    return await this.http.get(environment.API_URL + 'product-defect/getItemNo/' + _entity+'/'+_porNo).toPromise(); 

  } 

  public async getItemList(_entity: string , _porNo: string) {
    return await this.http.get(environment.API_URL + 'product-defect/getItemNoList/' + _entity+'/'+_porNo).toPromise(); 

  } 

  public async DataQcCutting(_model: DataQcCuttingView) {
    return await this.http.post(environment.API_URL + 'product-defect/postDataQcCutting', _model).toPromise();
  }
}
