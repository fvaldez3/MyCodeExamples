import * as Yup from 'yup';

const locationFormSchema = Yup.object().shape({
    lineOne: Yup.string().min(2).max(255).required('Is Required'),
    lineTwo: Yup.string().min(2).max(255).nullable(),
    city: Yup.string().min(2).max(255).required('Is Required'),
    zip: Yup.string().min(2).max(50).nullable(),
    stateId: Yup.number().min(1, 'Is Required'),
    locationTypeId: Yup.number().min(1, 'Is Required'),
});

export default locationFormSchema;