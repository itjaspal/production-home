import { Dropdownlist } from './../_model/dropdownlist';
import { environment } from './../../environments/environment';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class DropdownlistService {

  constructor(private http: HttpClient) { } 


  public async getDdlDefaultPrinter() {
    return await this.http.get<Dropdownlist[]>(environment.API_URL + 'dropdownlist/getDdlMobilePrnt').toPromise();
  }

  public async getDdlBranchStatus() {
    return await this.http.get<Dropdownlist[]>(environment.API_URL + 'dropdownlist/getDdlBranchStatus').toPromise();
  }

  public async getDdlBranchGroup() {
    return await this.http.get<Dropdownlist[]>(environment.API_URL + 'dropdownlist/getDdlBranchGroup').toPromise();
  }

  public async getDdlBranch(branchId: number = 0) {
    return await this.http.get<Dropdownlist[]>(environment.API_URL + 'dropdownlist/getDdlBranch/' + branchId).toPromise();
  }

  public async getDdlTransferBranch(branchId: number = 0) {
    return await this.http.get<Dropdownlist[]>(environment.API_URL + 'dropdownlist/getDdlTransferBranch/' + branchId).toPromise();
  }

  public async getDdlBranchInGroup(branchGroupId) {
    return await this.http.get<Dropdownlist[]>(environment.API_URL + 'dropdownlist/getDdlBranchInGroup/' + branchGroupId).toPromise();
  }

  public async getDdlBranchInGroupRpt(branchGroupId) {
    return await this.http.get<Dropdownlist[]>(environment.API_URL + 'dropdownlist/getDdlBranchInGroupRpt/' + branchGroupId).toPromise();
  }

  public async getDdlDepartments() {
    return await this.http.get<Dropdownlist[]>(environment.API_URL + 'dropdownlist/getDdlDepartment').toPromise();
  }

  public async getDdlUserRoles(_isPC, _createUser) {
        
    var ownerRole : any = {isPc : _isPC, createUser: _createUser };

    return await this.http.post(environment.API_URL + 'dropdownlist/inquiryDdlUserRole', ownerRole).toPromise();
  }

  // public async getDdlProductAttributesTypes() {
  //   return await this.http.get<Dropdownlist[]>(environment.API_URL + 'dropdownlist/getDdlProductAttributeType').toPromise();
  // }

  
  // public async getDdlProductAttribute(productAttributeTypeCode) {
  //   const params = new HttpParams()
  //     .set('productAttributeTypeCode', productAttributeTypeCode);
  //   return await this.http.get<Dropdownlist[]>(environment.API_URL + 'dropdownlist/getDdlProductAttribute', { params }).toPromise();
  // }

  public async getDdlProductGroup() { 
    
    return await this.http.get<Dropdownlist[]>(environment.API_URL + 'dropdownlist/getDdlProductGroup').toPromise();
  }

  public async getDdlProductType() { 
    
    return await this.http.get<Dropdownlist[]>(environment.API_URL + 'dropdownlist/getDdlProductType').toPromise();
  }

  public async getDdlProductBrand() { 
    
    return await this.http.get<Dropdownlist[]>(environment.API_URL + 'dropdownlist/getDdlProductBrand').toPromise();
  }

  public async getDdlProductDesign() { 
    
    return await this.http.get<Dropdownlist[]>(environment.API_URL + 'dropdownlist/getDdlProductDesign').toPromise();
  }

  public async getDdlProductModel() { 
    
    return await this.http.get<Dropdownlist[]>(environment.API_URL + 'dropdownlist/getDdlProductModel').toPromise();
  }

  public async getDdlProductColor() { 
    
    return await this.http.get<Dropdownlist[]>(environment.API_URL + 'dropdownlist/getDdlProductColor').toPromise();
  }

  public async getDdlProductSize() { 
    
    return await this.http.get<Dropdownlist[]>(environment.API_URL + 'dropdownlist/getDdlProductSize').toPromise();
  }
  
  public async getDdlProductAttributeRpt(productAttributeTypeCode) {
    const params = new HttpParams()
      .set('productAttributeTypeCode', productAttributeTypeCode);
    return await this.http.get<Dropdownlist[]>(environment.API_URL + 'dropdownlist/getDdlProductAttributeRpt', { params }).toPromise();
  }

  // public async getDdlSaleTransactionStatus() {
  //   return await this.http.get<Dropdownlist[]>(environment.API_URL + 'dropdownlist/getDdlSaleTransactionStatus').toPromise();
  // }

  // public async getDdlSalePC(_salePCId: string) {
  //   return await this.http.get<Dropdownlist[]>(environment.API_URL + 'dropdownlist/getDdlSalePC/' + _salePCId).toPromise();
  // }

  // public async getReturnProductReason() {
  //   return await this.http.get<Dropdownlist[]>(environment.API_URL + 'dropdownlist/getReturnProductReason').toPromise();
  // }

  // public async getDdlRequesitionTransactionStatus() {
  //   return await this.http.get<Dropdownlist[]>(environment.API_URL + 'dropdownlist/getDdlRequesitionTransactionStatus').toPromise();
  // }

  // public async getDdlDailySaleStatus() {
  //   return await this.http.get<Dropdownlist[]>(environment.API_URL + 'dropdownlist/getDdlDailySaleStatus').toPromise();
  // }

  // public async getDdlDocControlAddReturn() {
  //   return await this.http.get<Dropdownlist[]>(environment.API_URL + 'dropdownlist/getDdlDocControlAddReturn').toPromise();
  // }

  // public async getDdlFileUploadType() {
  //   return await this.http.get<Dropdownlist[]>(environment.API_URL + 'dropdownlist/getDdlFileUploadType').toPromise();
  // }

  // public async getDdlYear() {
  //   return await this.http.get<Dropdownlist[]>(environment.API_URL + 'dropdownlist/getDdlYear').toPromise();
  // }

  // public async getDdlMonth() {
  //   return await this.http.get<Dropdownlist[]>(environment.API_URL + 'dropdownlist/getDdlMonth').toPromise();
  // }

  // public async getDdlStockLocations() {
  //   return await this.http.get<Dropdownlist[]>(environment.API_URL + 'dropdownlist/getDdlStockLocation').toPromise();
  // }
}
