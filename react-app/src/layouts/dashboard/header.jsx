import PropTypes from 'prop-types';
import { Box, Stack, AppBar, Toolbar, IconButton } from '@mui/material';
import { useTheme } from '@mui/material/styles';
import useResponsive from '../../hooks/use-responsive';
import { bgBlur } from '../../theme/css';
import Iconify from '../../components/iconify';
import { NAV, HEADER } from './config-layout';
import NotificationsPopover from './common/notifications-popover';

export default function Header({ onOpenNav }) {
  const theme = useTheme();
  const lgUp = useResponsive('up', 'lg');

  const renderContent = (
    <>
      {!lgUp && (
        <IconButton
          onClick={onOpenNav}
          sx={{ mr: 1 }}
          aria-label="open navigation"
        >
          <Iconify icon="eva:menu-2-fill" />
        </IconButton>
      )}

      <Box sx={{ flexGrow: 1 }} />

      <Stack direction="row" alignItems="center" spacing={1}>
        <NotificationsPopover />
      </Stack>
    </>
  );

  return (
    <AppBar
      sx={{
        boxShadow: 'none',
        height: HEADER.H_MOBILE,
        zIndex: theme.zIndex.appBar + 1,
        ...bgBlur({
          color: theme.palette.background.default,
        }),
        transition: theme.transitions.create(['height'], {
          duration: theme.transitions.duration.shorter,
        }),
        ...(lgUp && {
          width: `calc(100% - ${NAV.WIDTH + 1}px)`,
          height: HEADER.H_DESKTOP,
        }),
      }}
    >
      <Toolbar
        sx={{
          height: 1,
          px: { lg: 5 },
        }}
      >
        {renderContent}
      </Toolbar>
    </AppBar>
  );
}

Header.propTypes = {
  onOpenNav: PropTypes.func.isRequired,
};
