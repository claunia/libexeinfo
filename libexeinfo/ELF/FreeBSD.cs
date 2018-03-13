namespace libexeinfo
{
    public partial class ELF
    {
        static FreeBSDTag DecodeFreeBSDTag(uint desc)
        {
            FreeBSDTag tag = new FreeBSDTag {major = desc / 100000};

            if(desc == 460002)
            {
                tag.major    = 4;
                tag.minor    = 6;
                tag.revision = 2;
            }
            else if(desc < 460100)
            {
                tag.minor = desc / 10000 % 10;
                if(desc          / 1000  % 10 > 0) tag.revision = desc / 1000 % 10;
            }
            else if(desc < 500000)
            {
                tag.minor = desc / 10000 % 10 + desc                        / 1000 % 10;
                if(desc                  / 10 % 10 > 0) tag.revision = desc / 10   % 10;
            }
            else
            {
                tag.minor = desc / 1000 % 100;
                if(desc          / 10   % 10 > 0) tag.revision = desc / 10 % 10;
            }

            return tag;
        }
    }
}