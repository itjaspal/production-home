import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'listFilter',
  pure: false
})
export class ListFilterPipe implements PipeTransform {
  transform(items: any[], keys: string, term: string): any {

    if (!term) return items;

    try {
      return (items || []).filter(item => keys.split(',').some(key => item.hasOwnProperty(key) && new RegExp(term, 'gi').test(item[key])));
    }
    catch (e) {
      return [];
    }
  }
}