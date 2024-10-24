import { format, formatDistanceToNow } from 'date-fns';

export function fDate(date) {
  return format(new Date(date), 'dd MMMM yyyy');
}

export function fDateTime(date) {
  return format(new Date(date), 'dd MMM yyyy HH:mm');
}

export function fTimestamp(date) {
  return format(new Date(date), 'dd MMM yyyy HH:mm:ss');
}

export function fToNow(date) {
  return formatDistanceToNow(new Date(date), {
    addSuffix: true,
  });
}

export function fSimpleDate(date) {
  return format(new Date(date), 'yyyy-MM-dd');
}

export function fTime(date) {
  return format(new Date(date), 'HH:mm');
}