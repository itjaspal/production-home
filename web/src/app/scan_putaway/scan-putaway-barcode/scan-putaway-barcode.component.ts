import { DeptDefaultView, PutAwayCancelSearchView, PutAwayDetailSearchView, PutAwayScanFinView, PutAwayScanSearchView, PutAwayScanView, PutAwayTotalView, VerifyLocView, WhDefaultView } from './../../_model/scan-putaway';
import { Dropdownlist } from './../../_model/dropdownlist';
import { AppSetting } from './../../_constants/app-setting';
import { ChangeDetectorRef, Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatDialog } from '@angular/material';
import { Router, ActivatedRoute } from '@angular/router';
import { AuthenticationService } from '../../_service/authentication.service';
import { DropdownlistService } from '../../_service/dropdownlist.service';
import { MessageService } from '../../_service/message.service';
import { ScanSendSearchView } from '../../_model/scan-send';
import { ScanPutawayService } from '../../_service/scan-putaway.service';
import { DatePipe } from '@angular/common';
import { Message } from '@angular/compiler/src/i18n/i18n_ast';

@Component({
  selector: 'app-scan-putaway-barcode',
  templateUrl: './scan-putaway-barcode.component.html',
  styleUrls: ['./scan-putaway-barcode.component.scss']
})
export class ScanPutawayBarcodeComponent implements OnInit {

  @ViewChild('bar_code') bar_code:ElementRef;
  @ViewChild('wc_code') wc_code:ElementRef;
  @ViewChild('wh_code') wh_code:ElementRef;
  @ViewChild('loc_code') loc_code:ElementRef;
  @ViewChild('qty') qty:ElementRef;

  constructor(
      private _fb: FormBuilder,
      private _authSvc: AuthenticationService,
      private _dialog: MatDialog,
      private _msgSvc: MessageService,
      private _router: Router,
      private _actRoute: ActivatedRoute,
      private cdr: ChangeDetectorRef,
      private _dll: DropdownlistService,
      private _putawaySvc: ScanPutawayService,

  ) { }

  public validationForm: FormGroup;
  public user: any;
  public vSum = 0;
  public wclist: Dropdownlist[] = [];
  public whlist: Dropdownlist[] = [];

  
  public model_ptwDetail: PutAwayDetailSearchView = new PutAwayDetailSearchView();
  public datas_ptwDetail: PutAwayTotalView<PutAwayScanView>= new PutAwayTotalView<PutAwayScanView>();

  public model_scan: PutAwayScanSearchView = new PutAwayScanSearchView();
  public datas_scan: PutAwayTotalView<PutAwayScanView>= new PutAwayTotalView<PutAwayScanView>();

  public current_scan: PutAwayScanFinView = new PutAwayScanFinView();
  public model_cancel: PutAwayCancelSearchView;

  public defaultWH: WhDefaultView = new WhDefaultView();
  public defaultDept: DeptDefaultView = new DeptDefaultView();
  public verifyLoc: VerifyLocView;

  async ngOnInit() {
    this.buildForm();
    this.user = this._authSvc.getLoginUser();

    //this.wclist = await this._dll.getDdlWCPtwByUser(AppSetting.entity, this.user.username);
    this.whlist = await this._dll.getDdlPutAwayWHMast();

    this.defaultWH = await this._putawaySvc.getWhDefault(AppSetting.entity);
    this.model_scan.wh_code = this.defaultWH.wh_code;

    //this.defaultDept = await this._putawaySvc.getDeptDefault(AppSetting.entity, this.user.username);
    //this.model_scan.wc_code = this.defaultDept.dept_code;

    
    this.model_ptwDetail.entity     = AppSetting.entity;
    this.model_ptwDetail.doc_no     = this._actRoute.snapshot.params.doc_no;
    this.model_ptwDetail.doc_code   = this._actRoute.snapshot.params.doc_code;
    this.model_ptwDetail.set_no     = "";
    this.model_ptwDetail.prod_code  = "";
    this.model_ptwDetail.bar_code   = "";

    this.datas_ptwDetail = await this._putawaySvc.searchPutAwayDetail(this.model_ptwDetail);
    //console.log(this.datas_ptwDetail);

    for (let x of this.datas_ptwDetail.ptwDetails) {
        let data: PutAwayScanView = new PutAwayScanView(); 
        data.item_no = x.item_no;
        data.set_no = x.set_no;
        data.prod_code = x.prod_code;
        data.bar_code = x.bar_code;
        data.prod_name = x.prod_name;
        data.wh_code = x.wh_code;
        data.loc_code = x.loc_code;
        data.qty = x.qty;
        this.add(data);
    }
    this.vSum  = this.datas_ptwDetail.total_qty;
    //console.log(this.current_scan.datas);
    this.loc_code.nativeElement.focus();
    
  }


  private buildForm() {
    this.validationForm = this._fb.group({
      bar_code: [null, [Validators.required]],
      wc_code: [null, [Validators.required]],
      wh_code: [null, [Validators.required]],
      loc_code: [null, [Validators.required]],
      qty: [null, [Validators.required]],
      set_no:[null, []]
    });
  }

  close() {
    window.history.back();
    //this._router.navigateByUrl('/scanPtw/');
    //scanPtw qr-add/
  }

  ngOnDestroy() {
    console.log("Close Program 2");

    sessionStorage.setItem('S2-doc_no', sessionStorage.getItem('S1-doc_no'));
    sessionStorage.setItem('S2-doc_date', sessionStorage.getItem('S1-doc_date'));
    sessionStorage.setItem('S2-doc_status', sessionStorage.getItem('S1-doc_status'));
    sessionStorage.setItem('Exit_Status', "OK");

    sessionStorage.removeItem('S1-doc_no');
    sessionStorage.removeItem('S1-doc_date');
    sessionStorage.removeItem('S1-doc_status');
  }

  /*async onLocCodeEntered(_locCode: string) {
     
    if (_locCode == null || _locCode == "") {
      return;
    }
    this.bar_code.nativeElement.focus();

  }*/

  async onBarCodeEntered(_barCode: string) {
     
    if (_barCode == null || _barCode == "") {
      return;
    }
    this.qty.nativeElement.focus();

  }

  async onQtyEntered(_qty: string) {

    if (_qty == null || _qty == ""  || _qty == "0") {
      return;
    }

    if (this.model_scan.wh_code == null || this.model_scan.wh_code == "" || this.model_scan.wh_code == "0") {
      return;
    }

    /*if (this.model_scan.wc_code == null || this.model_scan.wc_code == "" || this.model_scan.wc_code == "0") {
      return;
    }*/

    if (this.model_scan.loc_code == null || this.model_scan.loc_code == "") {
      return;
    }

    var datePipe = new DatePipe("en-US");
    
     //this.searchModel.req_date = this._actRoute.snapshot.params.req_date;
    /* this.model_scan.wc_code = this._actRoute.snapshot.params.wc_code;
     this.model_scan.bar_code = _qr;
     this.model_scan.req_date  = datePipe.transform(this._actRoute.snapshot.params.req_date, 'dd/MM/yyyy').toString();
    
     this.model_scan.user_id = this.user.username;
     this.model_scan.build_type = this.user.branch.entity_code;
     this.model_scan.pdjit_grp = this._actRoute.snapshot.params.pdjit_grp;*/
     
     this.model_scan.build_type  =  "";
     this.model_scan.doc_no      =  this._actRoute.snapshot.params.doc_no;
     this.model_scan.doc_code    =  this._actRoute.snapshot.params.doc_code;
     this.model_scan.doc_date    =  datePipe.transform(this._actRoute.snapshot.params.doc_date, 'dd/MM/yyyy').toString();
     this.model_scan.fr_wh_code  =  this._actRoute.snapshot.params.wh_code;
     this.model_scan.user_id     =  this.user.username;
     this.model_scan.qty         =  Number(_qty);

     //console.log( this.model_scan.doc_no + "|" + this.model_scan.doc_code + "|" + this.model_scan.doc_date + "|" + this.model_scan.wc_code + "|" + this.model_scan.fr_wh_code + "|" + this.model_scan.loc_code + "|" + this.model_scan.wh_code + "|" + this.model_scan.user_id + "|" + this.model_scan.bar_code + "|" + this.model_scan.qty );

    
     this.datas_scan = await this._putawaySvc.searchPutAwayManualAdd(this.model_scan);
     console.log(this.datas_scan); 

     for (let x of this.datas_scan.ptwDetails) {
          let data: PutAwayScanView = new PutAwayScanView(); 
          data.item_no = x.item_no;
          data.set_no = x.set_no;
          data.prod_code = x.prod_code;
          data.bar_code = x.bar_code;
          data.prod_name = x.prod_name;
          data.wh_code = x.wh_code;
          data.loc_code = x.loc_code;
          data.qty = x.qty;
          this.add(data);
          this.vSum  =  this.vSum  + x.qty;
     }
     this.bar_code.nativeElement.focus();
  }

  async onLocEntered(_locCode: string) {
    if (_locCode == null || _locCode == "") {
      return;
    }
    var datePipe = new DatePipe("en-US");
    this.verifyLoc = new VerifyLocView();

    this.verifyLoc = await this._putawaySvc.getVerifyLoc(_locCode);

    if (this.verifyLoc.loc_code != "" || this.verifyLoc.loc_code != null) {
       this.bar_code.nativeElement.focus(); 
    }else{
       this.loc_code.nativeElement.focus();
    }

  } 

  add(datas: any) {

        let newProd: PutAwayScanView = new PutAwayScanView();

        newProd.item_no = datas.item_no;
        newProd.set_no = datas.set_no;
        newProd.prod_code = datas.prod_code;
        newProd.bar_code = datas.bar_code;
        newProd.prod_name = datas.prod_name;
        newProd.wh_code = datas.wh_code;
        newProd.loc_code = datas.loc_code;
        newProd.qty = datas.qty;
        
        this.current_scan.datas.push(newProd);
        
        // Group By Product
      /* var result = [];
        this.model_scan.datas.forEach(function (a) {
          if ( !this[a.prod_code] && !this[a.prod_name] ) {
              this[a.prod_code] = { prod_code: a.prod_code, prod_name: a.prod_name, qty: 0 };
              result.push(this[a.prod_code]);
          } 
          this[a.prod_code].qty += a.qty;
          
        }, Object.create(null)); 
      
        this.model_scan.datas = result;*/
        //console.log(result)
        //console.log(this.model_scan.datas);

      }


      async PutAwayCancel(p_item: string, p_prodCode: string, p_barCode: string) {

          this.model_cancel = new PutAwayCancelSearchView();
    
          var datePipe = new DatePipe("en-US");
          //console.log( this._actRoute.snapshot.params.doc_no + "|" + this._actRoute.snapshot.params.doc_code + "|" + p_item + "|" + p_prodCode + "|" + p_barCode);
     
          this.model_cancel.doc_no     = this._actRoute.snapshot.params.doc_no;
          this.model_cancel.doc_code   = this._actRoute.snapshot.params.doc_code;
          this.model_cancel.item       = p_item;
          this.model_cancel.prod_code  = p_prodCode;
          this.model_cancel.bar_code   = p_barCode;

          this.datas_scan = await this._putawaySvc.searchPutAwayCancel(this.model_cancel);
          
          //Refresh Data
          //this.current_scan.datas.re
          this.current_scan =  new PutAwayScanFinView();
          this.model_ptwDetail.entity     = AppSetting.entity;
          this.model_ptwDetail.doc_no     = this._actRoute.snapshot.params.doc_no;
          this.model_ptwDetail.doc_code   = this._actRoute.snapshot.params.doc_code;
          this.model_ptwDetail.set_no     = "";
          this.model_ptwDetail.prod_code  = "";
          this.model_ptwDetail.bar_code   = "";

          this.datas_ptwDetail = await this._putawaySvc.searchPutAwayDetail(this.model_ptwDetail);
          //console.log(this.datas_ptwDetail);

          for (let x of this.datas_ptwDetail.ptwDetails) {
              let data: PutAwayScanView = new PutAwayScanView(); 
              data.item_no = x.item_no;
              data.set_no = x.set_no;
              data.prod_code = x.prod_code;
              data.bar_code = x.bar_code;
              data.prod_name = x.prod_name;
              data.wh_code = x.wh_code;
              data.loc_code = x.loc_code;
              data.qty = x.qty;
              this.add(data);
          }
          this.vSum  = this.datas_ptwDetail.total_qty; 
        
 
      }

}
