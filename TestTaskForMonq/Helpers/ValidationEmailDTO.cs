using EmailValidation;
using TestTaskForMonq.DtoControllerModels;

namespace TestTaskForMonq.Helpers
{
    /// <summary>
    /// Check EmailDTO on validation. Check body and recipient.
    /// </summary>
    public static class ValidationEmailDTO
    {
        public static bool IsValid(EmailDTO emailDTO)
        {
            if (string.IsNullOrWhiteSpace(emailDTO.Body)
                || emailDTO.Recipients == null)
            {
                return false;
            }

            //Validation recipient's email addresses
            foreach (var recipient in emailDTO.Recipients)
            {
                if (recipient == null
                    || !EmailValidator.Validate(recipient))
                {
                    return false;
                }
                //TODO Может быть сделать так:
                //занести в логи, что получатель невалиден,
                //удаляем этот адресс из массива получателей 
                //emailDTO.Recipients[recipientIndex]=emailDTO.Recipients[recipientIndex].Remove(0); - лишь опустошит элемент, но не удалит его
                //лучше найти индексы всех невалидных элементов и разом всё дропнуть
                //остальные адреса не трогать
            }
            return true;
        }
    }

}
