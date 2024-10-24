import PropTypes from 'prop-types';
import { FormProvider as RHFFormProvider } from 'react-hook-form';

const FormProvider = ({ children, onSubmit, methods }) => (
  <RHFFormProvider {...methods}>
    <form onSubmit={methods.handleSubmit(onSubmit)}>{children}</form>
  </RHFFormProvider>
);

FormProvider.propTypes = {
  children: PropTypes.node.isRequired,
  methods: PropTypes.object.isRequired,
  onSubmit: PropTypes.func.isRequired,
};

export default FormProvider;