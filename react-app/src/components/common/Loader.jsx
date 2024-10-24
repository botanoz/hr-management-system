import React from 'react';
import PropTypes from 'prop-types';
import { CircularProgress, Box, Typography } from '@mui/material';

const Loader = ({ size, message }) => (
  <Box
    display="flex"
    flexDirection="column"
    justifyContent="center"
    alignItems="center"
    height="100%"
  >
    <CircularProgress size={size} />
    {message && (
      <Typography variant="body2" sx={{ mt: 2 }}>
        {message}
      </Typography>
    )}
  </Box>
);

Loader.propTypes = {
  size: PropTypes.number,
  message: PropTypes.string,
};

Loader.defaultProps = {
  size: 40,
  message: '',
};

export default Loader;
