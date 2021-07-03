using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Contracts.Domain.Base;

namespace Domain.Base
{
    public class LangString : LangString<Guid, Translation>, IDomainEntityId
    {
        public LangString()
        {
        }

        public LangString(string value, string? culture = null) : base(value, culture)
        {
        }

        public static implicit operator string(LangString? langString) => langString?.ToString() ?? "Null";
        public static implicit operator LangString(string str) => new LangString(str);
    }

    public class LangString<TKey, TTranslation> : DomainEntityId<TKey>
        where TKey : IEquatable<TKey>
        where TTranslation : Translation<TKey>, new()
    {
        public LangString()
        {
        }


        public LangString(string value, string? culture = null)
        {
            SetTranslation(value, culture);
        }

        private const string DefaultCulture = "en";
        public virtual ICollection<TTranslation>? Translations { get; set; }


        public virtual void SetTranslation(string value, string? culture = null)
        {
            culture = culture?.Trim() ?? Thread.CurrentThread.CurrentUICulture.Name;


            if (Translations == null)
            {
                if (Id.Equals(default(TKey)))
                {
                    Translations = new List<TTranslation>();
                }
                else
                {
                    throw new NullReferenceException("You forgot to include Translations entity, Can't translate");
                }
            }

            var translation = Translations.FirstOrDefault(t => t.Culture == culture);
            if (translation == null)
            {
                Translations.Add(new TTranslation() {Value = value, Culture = culture});
            }
            else
            {
                translation.Value = value;
            }
        }

        public string? Translate(string? culture = null)
        {
            if (Translations == null)
            {
                if (Id.Equals(default))
                {
                    return null;
                }

                throw new NullReferenceException("You forgot to include Translations entity, Can't translate");
            }

            culture = culture?.Trim() ?? Thread.CurrentThread.CurrentUICulture.Name;
            /*
             Possible cultures in db
             en, en-GB
             In Query
             en, en-US, en-GB
             */


            // Exact match
            var translation = Translations?.FirstOrDefault(t => t.Culture == culture);
            if (translation != null)
            {
                return translation.Value;
            }

            // Exact default culture match
            translation = Translations?.FirstOrDefault(t => t.Culture == DefaultCulture);
            if (translation != null)
            {
                return translation.Value;
            }

            // Any match, f.e 'en-GB' doesn't exist, we look for 'en' culture
            translation = Translations?.FirstOrDefault(tran => culture.StartsWith(tran.Culture));
            if (translation != null)
            {
                return translation.Value;
            }

            translation = Translations?.FirstOrDefault(tran => DefaultCulture.StartsWith(tran.Culture));
            return translation != null ? translation.Value : Translations!.FirstOrDefault()?.Value;
        }

        public override string ToString()
        {
            return Translate() ?? "Something bad happened";
        }

        // langString -> string
        public static implicit operator string(LangString<TKey, TTranslation>? langString) =>
            langString?.ToString() ?? "Null";

        // string -> langString
        public static implicit operator LangString<TKey, TTranslation>(string str) =>
            new LangString<TKey, TTranslation>(str);
    }
}