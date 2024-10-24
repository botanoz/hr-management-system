import { styled, alpha } from '@mui/material/styles';
import { ListItemIcon, ListItemButton } from '@mui/material';

export const StyledNavItem = styled((props) => <ListItemButton disableGutters {...props} />)(
  ({ theme }) => ({
    ...theme.typography.body2,
    height: 56,
    position: 'relative',
    textTransform: 'capitalize',
    color: theme.palette.text.primary,
    borderRadius: theme.shape.borderRadius * 2,
    padding: theme.spacing(1, 2),
    marginBottom: theme.spacing(0.5),
    transition: theme.transitions.create(['background-color', 'box-shadow']),
    '&:hover': {
      backgroundColor: alpha(theme.palette.primary.main, 0.08),
      boxShadow: `0 0 0 1px ${alpha(theme.palette.primary.main, 0.24)}`,
    },
    '&.active': {
      color: theme.palette.primary.main,
      fontWeight: 'fontWeightBold',
      backgroundImage: `linear-gradient(135deg, ${alpha(theme.palette.primary.light, 0.16)} 0%, ${alpha(theme.palette.primary.dark, 0.16)} 100%)`,
      boxShadow: `0 0 0 1px ${alpha(theme.palette.primary.main, 0.24)}, 0 4px 8px 0 ${alpha(theme.palette.primary.main, 0.24)}`,
    },
  })
);

export const StyledNavItemIcon = styled(ListItemIcon)(({ theme }) => ({
  width: 26,
  height: 26,
  color: 'inherit',
  display: 'flex',
  alignItems: 'center',
  justifyContent: 'center',
  borderRadius: theme.shape.borderRadius,
  transition: theme.transitions.create('background-color'),
  '& svg': {
    fontSize: 20,
  },
  '.active &': {
    backgroundColor: alpha(theme.palette.primary.main, 0.16),
  },
}));