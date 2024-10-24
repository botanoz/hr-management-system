import { USER_ROLES } from './constants';

const permissions = {
  [USER_ROLES.ADMIN]: ['read:all', 'write:all', 'delete:all'],
  [USER_ROLES.MANAGER]: ['read:all', 'write:employees', 'write:leaves', 'write:expenses', 'write:shifts', 'write:holidays', 'write:companies'],
  [USER_ROLES.EMPLOYEE]: ['read:self', 'write:self'],
};

export function hasPermission(userRole, action) {
  if (!userRole || !action) return false;
  const userPermissions = permissions[userRole] || [];
  return userPermissions.includes(action) || userPermissions.includes('write:all');
}

export function canReadAll(userRole) {
  return hasPermission(userRole, 'read:all');
}

export function canWriteAll(userRole) {
  return hasPermission(userRole, 'write:all');
}

export function canDeleteAll(userRole) {
  return hasPermission(userRole, 'delete:all');
}

export function canManageEmployees(userRole) {
  return hasPermission(userRole, 'write:employees') || canWriteAll(userRole);
}

export function canManageLeaves(userRole) {
  return hasPermission(userRole, 'write:leaves') || canWriteAll(userRole);
}

export function canManageExpenses(userRole) {
  return hasPermission(userRole, 'write:expenses') || canWriteAll(userRole);
}

export function canManageShifts(userRole) {
  return hasPermission(userRole, 'write:shifts') || canWriteAll(userRole);
}

export function canManageHolidays(userRole) {
  return hasPermission(userRole, 'write:holidays') || canWriteAll(userRole);
}

export function canManageCompanies(userRole) {
  return hasPermission(userRole, 'write:companies') || canWriteAll(userRole);
}
