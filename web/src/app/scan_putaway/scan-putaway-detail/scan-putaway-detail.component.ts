import { PutAwayCancelSearchView, PutAwayDetailSearchView,  PutAwayScanView, PutAwayTotalView } from './../../_model/scan-putaway';
import { Dropdownlist } from './../../_model/dropdownlist';
import { AppSetting } from './../../_constants/app-setting';
import { ChangeDetectorRef, Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatDialog } from '@angular/material';
import { Router, ActivatedRoute } from '@angular/router';
import { AuthenticationService } from '../../_service/authentication.service';
import { DropdownlistService } from '../../_service/dropdownlist.service';
import { MessageService } from '../../_service/message.service';
import { ScanPutawayService } from '../../_service/scan-putaway.service';
import { DatePipe } from '@angular/common';
import { Message } from '@angular/compiler/src/i18n/i18n_ast';

@Component({
  selector: 'app-scan-putaway-detail',
  templateUrl: './scan-putaway-detail.component.html',
  styleUrls: ['./scan-putaway-detail.component.scss']
})
export class ScanPutawayDetailComponent implements OnInit {

  @ViewChild('set_no') set_no:ElementRef;
  @ViewChild('prod_code') prod_code:ElementRef;
  @ViewChild('bar_code') bar_code:ElementRef;

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
  public setNolist: Dropdownlist[] = [];
  public prodlist: Dropdownlist[] = [];

  
  public model_ptwDetail: PutAwayDetailSearchView = new PutAwayDetailSearchView();
  public datas_ptwDetail: PutAwayTotalView<PutAwayScanView>= new PutAwayTotalView<PutAwayScanView>();

  public datas_scan: PutAwayTotalView<PutAwayScanView>= new PutAwayTotalView<PutAwayScanView>();
  public model_cancel: PutAwayCancelSearchView;


  async ngOnInit() {
    this.buildForm();
    this.user = this._authSvc.getLoginUser();

    this.setNolist = await this._dll.getDdlPtwSetNoList(this._actRoute.snapshot.params.pd_entity, this._actRoute.snapshot.params.doc_code, this._actRoute.snapshot.params.doc_no);
    this.prodlist  = await this._dll.getDdlPtwProdList(this._actRoute.snapshot.params.pd_entity, this._actRoute.snapshot.params.doc_code, this._actRoute.snapshot.params.doc_no);
    
    this.search();

    
  }


  private buildForm() {
    this.validationForm = this._fb.group({
      set_no: [null, [Validators.required]],
      prod_code: [null, [Validators.required]],
      bar_code:[null, []]
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

    sessionStorage.removeItem('S1-doc_no');
    sessionStorage.removeItem('S1-doc_date');
    sessionStorage.removeItem('S1-doc_status');
  }

  async search() {
   

        this.model_ptwDetail.entity     = this._actRoute.snapshot.params.pd_entity;
        this.model_ptwDetail.doc_no     = this._actRoute.snapshot.params.doc_no;
        this.model_ptwDetail.doc_code   = this._actRoute.snapshot.params.doc_code;

        if (this.model_ptwDetail.set_no == "0"){ this.model_ptwDetail.set_no     = ""; }
        if (this.model_ptwDetail.prod_code == "0"){ this.model_ptwDetail.prod_code     = ""; }

     /*   this.model_ptwDetail.set_no     = ""; // this.set_no.nativeElement.focus();
        this.model_ptwDetail.prod_code  = "";
        this.model_ptwDetail.bar_code   = ""; */

        this.datas_ptwDetail = await this._putawaySvc.searchPutAwayDetail(this.model_ptwDetail);
        console.log(this.datas_ptwDetail);

    }

    async PutAwayCancel(p_item: string, p_prodCode: string, p_barCode: string) {

      this.model_cancel = new PutAwayCancelSearchView();

      var datePipe = new DatePipe("en-US");
      //console.log( this._actRoute.snapshot.params.doc_no + "|" + this._actRoute.snapshot.params.doc_code + "|" + p_item + "|" + p_prodCode + "|" + p_barCode);
 
      this.model_cancel.entity     = this._actRoute.snapshot.params.pd_entity;
      this.model_cancel.doc_no     = this._actRoute.snapshot.params.doc_no;
      this.model_cancel.doc_code   = this._actRoute.snapshot.params.doc_code;
      this.model_cancel.item       = p_item;
      this.model_cancel.prod_code  = p_prodCode;
      this.model_cancel.bar_code   = p_barCode;

      this.datas_scan = await this._putawaySvc.searchPutAwayCancel(this.model_cancel);
      this.search();

  }



}
