import { DataQcCuttingView, DataQcEnrtyView, ItemNoListView } from './../_model/product-defect';
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

  public async getItemQcEntryNo(_entity: string , _porNo: string , _qcProcess:string) {
    return await this.http.get(environment.API_URL + 'product-defect/getItemNoQcEntry/' + _entity+'/'+_porNo+'/'+_qcProcess).toPromise(); 
  } 

  public async getQcGroup(_buildType: string ) {
    return await this.http.get(environment.API_URL + 'product-defect/getQcGroup/' + _buildType).toPromise(); 
  } 


  public async getItemQcEntryList(_entity: string , _porNo: string , _qcProcess: string) {
    return await this.http.get(environment.API_URL + 'product-defect/getItemNoWipList/' + _entity+'/'+_porNo+'/'+_qcProcess).toPromise(); 

  } 

  public async DataQcEntry(_model: DataQcEnrtyView) {
    return await this.http.post(environment.API_URL + 'product-defect/postDataQcEntry', _model).toPromise();
  }

}
