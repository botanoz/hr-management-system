import PropTypes from 'prop-types';
import { forwardRef } from 'react';
import { Box, Link, Typography, useTheme } from '@mui/material';
import { RouterLink } from 'src/routes/components';

const Logo = forwardRef(({ disabledLink = false, sx, ...other }, ref) => {
  const theme = useTheme();

  const logoContent = (
    <Box
      ref={ref}
      sx={{
        display: 'inline-flex',
        alignItems: 'center',
        position: 'relative',
        ...sx,
      }}
      {...other}
    >
      <Box
        component="span"
        sx={{
          width: 40,
          height: 40,
          display: 'flex',
          alignItems: 'center',
          justifyContent: 'center',
          borderRadius: '50%',
          backgroundColor: theme.palette.primary.main,
          color: theme.palette.primary.contrastText,
          fontSize: 24,
          fontWeight: 'bold',
          mr: 1,
        }}
      >
        HR
      </Box>
      <Box>
        <Typography
          variant="h6"
          sx={{
            fontWeight: 700,
            color: theme.palette.text.primary,
            textTransform: 'uppercase',
            letterSpacing: 1,
            lineHeight: 1,
          }}
        >
          Human
        </Typography>
        <Typography
          variant="subtitle2"
          sx={{
            fontWeight: 500,
            color: theme.palette.primary.main,
            textTransform: 'uppercase',
            letterSpacing: 1,
            lineHeight: 1,
          }}
        >
          Resources
        </Typography>
      </Box>
      <Typography
        variant="caption"
        sx={{
          position: 'absolute',
          bottom: -10,
          right: 0,
          color: theme.palette.text.secondary,
          fontSize: 10,
          fontStyle: 'italic',
        }}
      >
        Bitirme Projesi
      </Typography>
    </Box>
  );

  if (disabledLink) {
    return logoContent;
  }

  return (
    <Link
      component={RouterLink}
      to="/"
      sx={{
        textDecoration: 'none',
        color: 'inherit',
        display: 'inline-flex',
      }}
    >
      {logoContent}
    </Link>
  );
});

Logo.propTypes = {
  disabledLink: PropTypes.bool,
  sx: PropTypes.object,
};

export default Logo;