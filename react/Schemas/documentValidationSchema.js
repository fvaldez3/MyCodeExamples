import * as Yup from 'yup';


const documentValidationSchema = Yup.object().shape({
    validationTypeId: Yup.number().min(2,"You must either accept or reject").required("Is required"),
    rejectMessage: Yup.string().when("validationTypeId", (validationTypeId,field)=>

{    if(validationTypeId === 3) 
    {
        return field.min(2,"Must enter more than 2 characters")
        .max(4000,"Max amout of characters is 4000")
        .required("Is Required");

    }
    else {return field}
}
    )
});

export default documentValidationSchema;